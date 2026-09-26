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
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Wages Exempt Item Schedule - Architecture.WinApp.Lookups.WagesExemptItemSchedule (desktop), the
 * procedures it calls with its own parameters.
 *
 *   ItemBind            CommonServices.ItemGetAllServiceBind() -> Item.GetAll (BLL): @OrganizationId,
 *                       @CompanyId, @Activity='ReadByOrganizationCompanyId'; @ParentIds omitted (null).
 *   WagesAccountBind    CommonServices.GetWagesAccount(null, 4, 1) -> InvConractorWagesAccounts
 *                       .GetWagesItemsByWagesTypeIds: @OrganizationId, @CompanyId, @WagesActivityId=4,
 *                       @ActionId=1, @Activity='GetWagesItemsByWagesTypeIds'; @WagesLookupIds omitted (null).
 *   DocumentType        CommonServices.StaticColumnsService("DocTypeForWagesExemptItemSchedule") ->
 *                       GeneralReprots.StaticColumnNames: SpStaticColumnNames @Activity.
 *   BindHistory         WagesExemptItemSchedule.FormHistory (BLL): @OrganizationId, @CompanyId always;
 *                       @EffectedFrom / @EffectedTo (CheckDateTimeNull), @ItemId, @RefDocumentTypeId (!= 0)
 *                       are all unset on the form's model, so never sent. The procedure's name has a space.
 *   Save                WagesExemptItemSchedule.Save (BLL): Id == 0 -> USp_WagesExemptItemSchedule_Insert,
 *                       else _Update; DAL SetData -> GenericProvider.SetProc in its own transaction:
 *                       EVERY model property is sent (AddWithValue, reflection), then ExecuteScalar and
 *                       Convert.ToInt32 (Insert: SCOPE_IDENTITY(); Update: no result set -> 0).
 */
@Repository
public class ProductionWagesExemptRepository {

    private final JdbcTemplate jdbc;

    public ProductionWagesExemptRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> items(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("Sp_Item_GetAllMethod", p);
    }

    public List<Map<String, Object>> wagesAccounts(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@WagesActivityId", 4);
        p.put("@ActionId", 1);
        p.put("@Activity", "GetWagesItemsByWagesTypeIds");
        return rows("Sp_InvConractorWagesAccounts_GetAllMethod", p);
    }

    public List<Map<String, Object>> documentTypes() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Activity", "DocTypeForWagesExemptItemSchedule");
        return rows("SpStaticColumnNames", p);
    }

    public List<Map<String, Object>> history(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        return rows("[dbo].[USp_WagesExemptItemSchedule_Form History]", p);
    }

    /** The sixteen model properties, all of them, as SetProc sends them. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> model) {
        int id = model.get("@Id") instanceof Number ? ((Number) model.get("@Id")).intValue() : 0;
        String proc = id == 0 ? "USp_WagesExemptItemSchedule_Insert" : "USp_WagesExemptItemSchedule_Update";
        List<Map<String, Object>> r = rows(proc, model);
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;           // Convert.ToInt32(null) = 0
        Object v = r.get(0).values().iterator().next();
        if (v == null) return 0;
        if (v instanceof Number) return (int) Math.round(((Number) v).doubleValue());
        try { return (int) Math.round(Double.parseDouble(String.valueOf(v).trim())); }
        catch (NumberFormatException e) { return 0; }
    }

    // ============================================================================= plumbing

    /** One EXEC with named parameters; the first result set is read, the rest are drained. */
    private List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
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
                for (int i = 0; i < values.size(); i++) {
                    Object v = values.get(i);
                    if (v instanceof LocalDateTime) ps.setTimestamp(i + 1, Timestamp.valueOf((LocalDateTime) v));
                    else ps.setObject(i + 1, v);
                }
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
