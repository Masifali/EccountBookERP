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
 * The four Taxation master-data definitions (dbo.ScreenDefinition ModuleId 9, App 13 "Taxation"):
 *
 *   178 InvfrmAddTaxType                     -> Architecture.BLL.Inventory.TaxesTypes                        (BLL 0604 / DAL 0457 / Model 1069)
 *   177 InvfrmAddTaxSchedule                 -> Architecture.BLL.Inventory.TaxScheduleMain                   (BLL 0608 / DAL 0461 / Model 1073)
 *   172 frmTaxNotesAndGLMaping               -> Architecture.BLL.Inventory.TaxSalesNotesGLMaping             (BLL 0607 / DAL 0460 / Model 1072)
 *                                              + Architecture.BLL.Inventory.TaxLookUps                       (BLL 0605)
 *   173 frmSupplierCustomerExemptionSchedule -> Architecture.BLL.Inventory.SupplierCustomerExemptionSchedule (BLL 0601 / DAL 0454 / Model 1066)
 *
 * Every call is the desktop's own procedure with the desktop's own parameters, read from
 * procdure.utf8.sql on 29-Sep-2026:
 *
 *   Sp_TaxesTypes_GetAllMethod           @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId' (TaxesTypes.Getall)
 *                                        @Id @Activity='ReadById'                                           (TaxesTypes.GetByID)
 *   Sp_TaxesTypes_Insert / _Update       the model's 11 properties (GenericProvider.SetProc sends every one)
 *   Sp_COAAllocation_GetAllMethod        @OrganizationId @CompanyId @Activity='COAAllocationSearch'  (COAAllocation.GetLst - Add Tax Type's account combo,
 *                                        the form drops AccountTypeId 2, 11 and 15)
 *                                        @OrganizationId @CompanyId @UserId @Activity='COAForCombobindig' (CommonServices.CoaAllocationGetForComboServiceBind -
 *                                        Tax Notes' GL Account combo)
 *   Sp_TaxScheduleMain_GetAllMethod      @Activity='ReadAll' @OrganizationId @CompanyId                (TaxScheduleMain.GetAll)
 *                                        @Id @Activity='ReadById'                                     (TaxScheduleMain.GetByID)
 *   Sp_TaxScheduleMain_Insert / _Update  the model's 13 properties. Both RAISERROR when the Tax Type already has a
 *                                        schedule; Update also refuses a Tax Type used in Vouchers / PurchaseOrder /
 *                                        InvPurchaseInvoice. The desktop shows that text; so does the page.
 *   Sp_TaxLookUps_GetAllMethod           @OrganizationId @CompanyId @TaxLookUptypesId @Activity='ReadByOrganizationCompanyIdNTaxeLookUpTypeId'
 *                                        (TaxLookUps.GetByTaxLookTypeId - type 1 = Sale Tax Notes, type 2 = Input / Output)
 *   Sp_TaxSalesNotesGLMaping_GetAllMethod @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId' (Getall)
 *                                        @Id @Activity='ReadById' (GetByID) - NOTE the procedure's own predicate is
 *                                        "where Id = Id", so it returns EVERY row and the desktop's GetByID picks [0],
 *                                        i.e. the first row of the table, not the one double-clicked. This port reads
 *                                        the listed rows and picks the one with the requested Id instead (see service).
 *   Sp_TaxSalesNotesGLMaping_Insert / _Update  the model's 13 properties
 *   Sp_SupplierCustomer_GetAllMethod     @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId' (SupplierCustomer.Getall)
 *   Sp_SupplierCustomerExemptionSchedule_GetAllMethod  @Activity='ReadAll' (+ @OrganizationId @CompanyId, see below)
 *                                        @Id @Activity='ReadById'
 *   Sp_SupplierCustomerExemptionSchedule_Insert / _Update  see below
 *
 * SupplierCustomerExemptionSchedule: the procedures as dumped are NEWER than the decompiled BLL/Model. The
 * BLL sends only @Activity='ReadAll' and the model has no OrganizationId / CompanyId / IsActive / audit
 * columns, while the procedure filters ReadAll on @OrganizationId/@CompanyId (so the desktop's history grid
 * comes back empty against this database) and Insert writes IsActive, EntryDate, EntryUserId,
 * OrganizationId, CompanyId. This port sends what the procedure expects - the session's organisation and
 * company, IsActive = 1 (the form has no checkbox for it), and the audit columns - the same "newer resolved
 * source" rule applied to Store Management 341.
 *
 * Save: DAL SetData = GenericProvider.SetProc in its own transaction, ExecuteScalar -> Convert.ToInt32.
 * The Insert procedures SELECT the new Id (MAX+1); the Update procedures return nothing, so an update
 * answers with the record's own Id (SetDate's fallback).
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class TaxationMasterRepository {

    private final JdbcTemplate jdbc;

    public TaxationMasterRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================ 178 Add Tax Type (TaxesTypes)

    /** TaxesTypes.Getall - joined to ChartofAccount, so AccountTitle comes back. */
    public List<Map<String, Object>> taxTypes(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("Sp_TaxesTypes_GetAllMethod", p);
    }

    /** TaxesTypes.GetByID. */
    public List<Map<String, Object>> taxTypeById(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        return rows("Sp_TaxesTypes_GetAllMethod", p);
    }

    /** TaxesTypes.Save: Id == 0 -> Insert, else Update; every model property sent. */
    @Transactional(rollbackFor = Exception.class)
    public int saveTaxType(Map<String, Object> model) {
        int id = asInt(model.get("@Id"));
        return setData(id == 0 ? "Sp_TaxesTypes_Insert" : "Sp_TaxesTypes_Update", model, id);
    }

    /** COAAllocation.GetLst -> Sp_COAAllocation_GetAllMethod 'COAAllocationSearch' (Id, AccountTitle, AccountTypeId ...). */
    public List<Map<String, Object>> coaAllocationSearch(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "COAAllocationSearch");
        return rows("Sp_COAAllocation_GetAllMethod", p);
    }

    /** CommonServices.CoaAllocationGetForComboServiceBind -> COAAllocation.GetForComboBind 'COAForCombobindig'. */
    public List<Map<String, Object>> coaForCombo(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (u.getId() != 0) p.put("@UserId", u.getId());
        p.put("@Activity", "COAForCombobindig");
        return rows("Sp_COAAllocation_GetAllMethod", p);
    }

    // ============================================================ 177 Add Tax Schedule (TaxScheduleMain)

    /** TaxScheduleMain.GetAll - @Activity first, as the BLL builds its list. */
    public List<Map<String, Object>> taxSchedules(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Activity", "ReadAll");
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        return rows("Sp_TaxScheduleMain_GetAllMethod", p);
    }

    /** TaxScheduleMain.GetByID. */
    public List<Map<String, Object>> taxScheduleById(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        return rows("Sp_TaxScheduleMain_GetAllMethod", p);
    }

    /** TaxScheduleMain.Save: Id == 0 -> Insert, else Update. */
    @Transactional(rollbackFor = Exception.class)
    public int saveTaxSchedule(Map<String, Object> model) {
        int id = asInt(model.get("@Id"));
        return setData(id == 0 ? "Sp_TaxScheduleMain_Insert" : "Sp_TaxScheduleMain_Update", model, id);
    }

    // ============================================================ 172 Tax Notes and GL Maping

    /** TaxLookUps.GetByTaxLookTypeId(Id = typeId) - the Id on the model is sent as @TaxLookUptypesId. */
    public List<Map<String, Object>> taxLookUps(UserAccount u, int typeId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@TaxLookUptypesId", typeId);
        p.put("@Activity", "ReadByOrganizationCompanyIdNTaxeLookUpTypeId");
        return rows("Sp_TaxLookUps_GetAllMethod", p);
    }

    /** TaxSalesNotesGLMaping.Getall - with saletaxnotes, inputoutput and AccountTitle joined. */
    public List<Map<String, Object>> glMapings(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("Sp_TaxSalesNotesGLMaping_GetAllMethod", p);
    }

    /** TaxSalesNotesGLMaping.Save: Id == 0 -> Insert, else Update. */
    @Transactional(rollbackFor = Exception.class)
    public int saveGlMaping(Map<String, Object> model) {
        int id = asInt(model.get("@Id"));
        return setData(id == 0 ? "Sp_TaxSalesNotesGLMaping_Insert" : "Sp_TaxSalesNotesGLMaping_Update", model, id);
    }

    // ============================================================ 173 Supplier Customer Tax Exemption

    /** SupplierCustomer.Getall -> Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyId' (Id, CompanyName ...). */
    public List<Map<String, Object>> supplierCustomers(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** SupplierCustomerExemptionSchedule.Getall - 'ReadAll', with the organisation and company the procedure filters on. */
    public List<Map<String, Object>> exemptions(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Activity", "ReadAll");
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        return rows("Sp_SupplierCustomerExemptionSchedule_GetAllMethod", p);
    }

    /** SupplierCustomerExemptionSchedule.GetByID. */
    public List<Map<String, Object>> exemptionById(int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        return rows("Sp_SupplierCustomerExemptionSchedule_GetAllMethod", p);
    }

    /** SupplierCustomerExemptionSchedule.Save: Id == 0 -> Insert, else Update. Both SELECT @Id AS Id. */
    @Transactional(rollbackFor = Exception.class)
    public int saveExemption(Map<String, Object> model) {
        int id = asInt(model.get("@Id"));
        return setData(id == 0 ? "Sp_SupplierCustomerExemptionSchedule_Insert" : "Sp_SupplierCustomerExemptionSchedule_Update", model, id);
    }

    // ============================================================================= plumbing

    /**
     * DAL SetData: ExecuteScalar -> Convert.ToInt32; > 0 becomes the new Id, otherwise the model's own Id.
     * An SQL RAISERROR propagates with the procedure's own message.
     */
    private int setData(String proc, Map<String, Object> params, int fallbackId) {
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
