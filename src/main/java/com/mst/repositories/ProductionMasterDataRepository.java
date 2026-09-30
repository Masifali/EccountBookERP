package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.LinkedCaseInsensitiveMap;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The three Production master-data definitions (Architecture.WinApp.Production):
 *
 *   DefineProductionType      -> Architecture.BLL.Production.ProductionType      (0327) / DAL 0265 / Model 0466
 *   DefineProductionPlanType  -> Architecture.BLL.Production.ProductionPlanType  (0326) / DAL 0264 / Model 0465
 *   DefineProductionPlant     -> Architecture.BLL.Production.InvProductionPlant  (0336) / DAL 0274 / Model 0482
 *
 * Every call below is the desktop's own procedure with the desktop's own parameters, checked against
 * procdure.utf8.sql (procdure_index.csv lines 214845-215040 and 176434-176600):
 *
 *   Sp_ProductionType_GetAllMethod       @Id                              (BLL GetAll: @Id only when Id > 0)
 *   Sp_ProductionType_Insert / _Update   @Id @ProductionTypeCode @ProductionTypeDescription
 *   Sp_ProductionPlanType_GetAllMethod   @Id                              (BLL GetAll: @Id only when Id > 0)
 *   Sp_ProductionPlanType_Insert/_Update @Id @PlanTypeCode @PlanTypeDescription
 *   Sp_InvProductionPlant_GetAllMethod   @OrganizationId @CompanyId @Activity='GetALL'   (BLL GetAll)
 *                                        @Activity='GetById' @Id                          (BLL GetById)
 *   Sp_InvProductionPlant_Insert/_Update @Id @Code @Description @OrganizationId @CompanyId @BranchId
 *                                        @ProjectId @ActionId  - the model's eight properties, all sent
 *   Sp_Branches_GetAllMethod             @OrganizationId @CompanyId @Activity='GetAll'    (CommonServices.BrancheServiceBind)
 *
 * Save (DAL SetDate): GenericProvider.SetProc in its own transaction, every model property sent
 * (AddWithValue by reflection), ExecuteScalar -> Convert.ToInt32; when that is not > 0 the DAL returns
 * obj.Id instead. Insert procedures return SCOPE_IDENTITY() (ProductionType: its own MAX(Id)+1); the
 * Update procedures return no result set, so an update answers with the record's own Id.
 *
 * Two facts about the procedures as dumped on 23-Sep-2026, reproduced and not corrected here:
 *  - Sp_ProductionType_Update sets ProductionTypeDescription only; ProductionTypeCode keeps its old
 *    value (Sp_ProductionPlanType_Update sets both).
 *  - Sp_InvProductionPlant_Insert and _Update begin with RAISERROR('Record cannot be inserted' /
 *    'Record cannot be updated', 16, 1) and RETURN. The desktop shows that message; so does the page.
 *
 * ProductionType.GetByID (BLL) sends @Activity='ReadById' to a procedure that has no @Activity
 * parameter; no form calls it (DefineProductionType uses GetAll with an Id), so it is not ported.
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ProductionMasterDataRepository {

    private final JdbcTemplate jdbc;

    public ProductionMasterDataRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================ Production Type (DefineProductionType)

    /** ProductionType.GetAll(new ProductionType { Id }) - @Id only when > 0. */
    public List<Map<String, Object>> productionTypes(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (id > 0) p.put("@Id", id);
        return rows("Sp_ProductionType_GetAllMethod", p);
    }

    /** ProductionType.Save: Id == 0 -> Insert, else Update. Both columns carry the same text (form :80-84, :171). */
    @Transactional(rollbackFor = Exception.class)
    public int saveProductionType(int id, String code, String description) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", id);
        p.put("@ProductionTypeCode", code);
        p.put("@ProductionTypeDescription", description);
        return setDate(id == 0 ? "Sp_ProductionType_Insert" : "Sp_ProductionType_Update", p, id);
    }

    // ============================================================ Plan Type (DefineProductionPlanType)

    /** ProductionPlanType.GetAll(new ProductionPlanType { Id }) - @Id only when > 0. */
    public List<Map<String, Object>> planTypes(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        if (id > 0) p.put("@Id", id);
        return rows("Sp_ProductionPlanType_GetAllMethod", p);
    }

    /** ProductionPlanType.Save: Id == 0 -> Insert, else Update. */
    @Transactional(rollbackFor = Exception.class)
    public int savePlanType(int id, String code, String description) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", id);
        p.put("@PlanTypeCode", code);
        p.put("@PlanTypeDescription", description);
        return setDate(id == 0 ? "Sp_ProductionPlanType_Insert" : "Sp_ProductionPlanType_Update", p, id);
    }

    // ============================================================ Plant (DefineProductionPlant)

    /**
     * InvProductionPlant.GetAll (BLL :30-52): @OrganizationId, @CompanyId, @Activity='GetALL'. The form
     * also sets BranchId = UserAccount.BranchesId on the model, but the BLL never sends it and the
     * procedure's GetALL branch does not filter on it - all of the company's plants are listed.
     */
    public List<Map<String, Object>> plants(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "GetALL");
        return rows("Sp_InvProductionPlant_GetAllMethod", p);
    }

    /** InvProductionPlant.GetById (BLL :54-72): @Activity='GetById', @Id - in that order. */
    public List<Map<String, Object>> plantById(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Activity", "GetById");
        p.put("@Id", id);
        return rows("Sp_InvProductionPlant_GetAllMethod", p);
    }

    /** InvProductionPlant.Save - the model's eight properties, every one sent. */
    @Transactional(rollbackFor = Exception.class)
    public int savePlant(Map<String, Object> model) {
        int id = asInt(model.get("@Id"));
        return setDate(id == 0 ? "Sp_InvProductionPlant_Insert" : "Sp_InvProductionPlant_Update", model, id);
    }

    /** CommonServices.BrancheServiceBind -> Branches.GetAll: Sp_Branches_GetAllMethod @Activity='GetAll'. */
    public List<Map<String, Object>> branches(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "GetAll");
        return rows("Sp_Branches_GetAllMethod", p);
    }

    /**
     * btnWarehouseAllocation_Click: CommonServices.SetRightsValueInRightsObject("frmWarehousesAllocationToPlant")
     * .DoHaveViewRight - the "View" right of that screen for this user and company, from the real chain
     * (tblUserRights -> ScreenRights 'View' -> ScreenDefinition by ScreenName). Read-only.
     */
    public boolean hasViewRight(UserAccount u, String screenName) {
        Integer n = jdbc.queryForObject(
                "SELECT COUNT(*) FROM dbo.tblUserRights ur "
              + "INNER JOIN dbo.ScreenRights sr ON sr.Id = ur.RightId "
              + "INNER JOIN dbo.ScreenDefinition sd ON sd.Id = sr.ScreenID "
              + "WHERE ur.UserId = ? AND ur.CompanyId = ? AND ur.Value = 1 "
              + "AND sr.RightName = 'View' AND sd.ScreenName = ?",
                Integer.class, u.getId(), u.getCompanyId(), screenName);
        return n != null && n > 0;
    }

    // ============================================================================= plumbing

    /**
     * DAL SetDate: ExecuteScalar -> Convert.ToInt32; > 0 becomes the new Id, otherwise the model's own
     * Id is the answer. An SQL error (the plant procedures' RAISERROR) propagates with its message.
     */
    private int setDate(String proc, Map<String, Object> params, int fallbackId) {
        List<Map<String, Object>> r = rows(proc, params);
        int num = 0;
        if (!r.isEmpty() && !r.get(0).isEmpty()) {
            Object v = r.get(0).values().iterator().next();
            if (v instanceof Number) num = (int) Math.round(((Number) v).doubleValue());
            else if (v != null) {
                try { num = (int) Math.round(Double.parseDouble(String.valueOf(v).trim())); }
                catch (NumberFormatException e) { num = 0; }
            }
        }
        return num > 0 ? num : fallbackId;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    /** One EXEC with named parameters; the first result set is read, the rest are drained. */
    private List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        final String text = sql.toString();
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            try (PreparedStatement ps = con.prepareStatement(text)) {
                for (int i = 0; i < values.size(); i++) ps.setObject(i + 1, values.get(i));
                boolean isRs = ps.execute();
                List<Map<String, Object>> out = null;
                while (true) {
                    if (isRs) {
                        try (ResultSet rs = ps.getResultSet()) {
                            if (out == null) out = read(rs);
                            else while (rs.next()) { /* drain */ }
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    isRs = ps.getMoreResults();
                }
                return out == null ? new ArrayList<>() : out;
            }
        });
    }

    private static List<Map<String, Object>> read(ResultSet rs) throws SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> row = new LinkedCaseInsensitiveMap<>(n);
            for (int c = 1; c <= n; c++) {
                String name = md.getColumnLabel(c);
                if (name == null || name.isEmpty()) name = md.getColumnName(c);
                if (name == null || name.isEmpty()) name = "Column" + c;
                Object v = rs.getObject(c);
                if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof java.sql.Date) v = ((java.sql.Date) v).toLocalDate().toString();
                row.put(name, v);
            }
            out.add(row);
        }
        return out;
    }
}
