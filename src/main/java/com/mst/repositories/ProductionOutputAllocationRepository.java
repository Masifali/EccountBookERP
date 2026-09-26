package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * "Production Output Allocation With Export Invoice" -
 * Architecture.WinApp.Production/ProductionOutputAllocationWithExportInvoice.cs (1,724 lines).
 * Opened by frmProductionSettlement.btnAllocateExportInvoiceToJobOrder_Click:2206.
 *
 * Every read and write the form performs, traced form line -> BLL -> procedure. BLL guards are
 * reproduced literally (omitted, never sent as NULL/0). Every name sent is declared by its
 * procedure (checked against procdure.utf8.sql).
 */
@Repository
public class ProductionOutputAllocationRepository {

    /** base.Name - the rights screen name (SetRightsValueInRightsObject(ScreenName)). */
    public static final String DESKTOP_SCREEN_NAME = "ProductionOutputAllocationWithExportInvoice";

    public static final String GET_ALL_PROC = "USP_ProductionOutputAllocationWithExportInvoice_GetAllMethod";

    @Autowired private JdbcTemplate jdbc;

    /* ================================================================================ rights */

    /** CommonServices.SetRightsValueInRightsObject -> tblUserRights.GetByUserId (Sp_tblUserRights_GetAllMethod). */
    public List<Map<String, Object>> userRightsForScreen(int userId, String roleName, int companyId) {
        return rows("dbo.Sp_tblUserRights_GetAllMethod",
                p("@UserId", userId, "@ScreenName", DESKTOP_SCREEN_NAME,
                  "@RightName", roleName == null ? "" : roleName,
                  "@CompanyId", companyId, "@Activity", "GetByUserId"));
    }

    /* =============================================================================== pickers */

    /**
     * GetJobOrderNoData:206 -> InvProductionJobOrder.GetJobOrderAll(FinancialYearId = ActiveYr.Id, ActionId = 1)
     * -> Sp_InvProductionJobOrder_GetAllMethod: @OrganizationId, @CompanyId always; @FinancialYearId,
     * @DocumentTypeId (never set here), @ActionId only when != 0; @Activity='GetJobOrderNoAll'.
     */
    public List<Map<String, Object>> jobOrdersAll(int organizationId, int companyId, int financialYearId, int actionId) {
        LinkedHashMap<String, Object> m = p("@OrganizationId", organizationId, "@CompanyId", companyId);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (actionId != 0) m.put("@ActionId", actionId);
        m.put("@Activity", "GetJobOrderNoAll");
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod", m);
    }

    /**
     * GetHistoryComboData:242 -> GetDataForDropDownFromProductionOutputAllocationWithExportInvoice
     * -> [dbo].[USP_GetDataForDropDownFromProductionOutputAllocationWithExportInvoice] @OrganizationId, @CompanyId
     * (no @Activity: every bucket comes back, the form keeps Activity = "JobOrder").
     */
    public List<Map<String, Object>> historyDropDown(int organizationId, int companyId) {
        return rows("[dbo].[USP_GetDataForDropDownFromProductionOutputAllocationWithExportInvoice]",
                p("@OrganizationId", organizationId, "@CompanyId", companyId));
    }

    /**
     * CommonServices.GetExportInvoiceNo() (SupplierCustomerId 0, FinancialYearId 0) -> ExImInvoice.GetInvoiceNo
     * -> Sp_ExImInvoice_GetAllMethod: @OrganizationId, @CompanyId; @SupplierCustomerId / @FinancialYearId
     * only when != 0 (both omitted here); @Activity='InvoiceNo'.
     */
    public List<Map<String, Object>> exportInvoiceNos(int organizationId, int companyId) {
        return rows("dbo.Sp_ExImInvoice_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@Activity", "InvoiceNo"));
    }

    /**
     * GetDataAgainstJobOrderFromProduction:643 -> GetDataAgainstJobOrder -> USP_GetOutPutItemsWithSumOfQtyWeight
     * @OrganizationId, @CompanyId, @JobOrderId (unguarded).
     */
    public List<Map<String, Object>> outputItems(int organizationId, int companyId, int jobOrderId) {
        return rows("dbo.USP_GetOutPutItemsWithSumOfQtyWeight",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@JobOrderId", jobOrderId));
    }

    /* =============================================================================== history */

    /**
     * GetHistoryData:768 -> FormHistory -> [dbo].[USP_ProductionOutputAllocationWithExportInvoice_GetAllMethod]:
     * @OrganizationId, @CompanyId, @BranchesId, @FinancialYearId, @CanViewAllRecord always;
     * @FromDate / @ToDate when set; @JobOrderId when != 0; @Activity='FormHistory'.
     * (The BLL's @EntryUser - sent only when CanViewAllRecord is false - is not declared by the
     * procedure; the service reproduces that call's failure without sending it.)
     */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId, int branchesId,
                                                 int financialYearId, boolean canViewAllRecord,
                                                 Timestamp fromDate, Timestamp toDate, int jobOrderId) {
        LinkedHashMap<String, Object> m = p("@OrganizationId", organizationId, "@CompanyId", companyId,
                "@BranchesId", branchesId, "@FinancialYearId", financialYearId,
                "@CanViewAllRecord", canViewAllRecord);
        if (fromDate != null) m.put("@FromDate", fromDate);
        if (toDate != null) m.put("@ToDate", toDate);
        if (jobOrderId != 0) m.put("@JobOrderId", jobOrderId);
        m.put("@Activity", "FormHistory");
        return rows("[dbo].[" + GET_ALL_PROC + "]", m);
    }

    /**
     * ReadById:955 -> GetByID(JobOrderId) -> [dbo].[USP_ProductionOutputAllocationWithExportInvoice_GetAllMethod]
     * @JobOrderId, @Activity='ReadById' - exactly these two; the BLL sends no tenancy here.
     */
    public List<Map<String, Object>> readById(int jobOrderId) {
        return rows("[dbo].[" + GET_ALL_PROC + "]", p("@JobOrderId", jobOrderId, "@Activity", "ReadById"));
    }

    /**
     * Slip:1024 -> ProductionOutputAllocationWithExportInvoice_SlipAndRegister(JobOrderId, 0)
     * -> [dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]:
     * @JobOrderId when > 0, @ExImInvoiceId when > 0 (0 here - omitted).
     */
    public List<Map<String, Object>> slipAndRegister(int jobOrderId) {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        if (jobOrderId > 0) m.put("@JobOrderId", jobOrderId);
        return rows("[dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]", m);
    }

    /* ================================================================================= write */

    /**
     * Insert():384 -> ProductionOutputAllocationWithExportInvoice.Save
     * -> DAL SetData(obj, "[dbo].[USP_ProductionOutputAllocationWithExportInvoice_InsertAndUpdate]"):
     * ONE SqlTransaction; for each item of ProductionOutputAllocationWithExportInvoicesList
     * EntryDate = ModifyDate = DateTime.Now, then GenericProvider ExecuteScalar with every model
     * property the procedure declares; commit after the loop, rollback on the first failure.
     *
     * @return the last call's scalar (Convert.ToInt32; a procedure branch that SELECTs nothing gives 0).
     */
    public int save(List<LinkedHashMap<String, Object>> items) {
        return jdbc.execute((ConnectionCallback<Integer>) con -> {
            boolean auto = con.getAutoCommit();
            con.setAutoCommit(false);
            try {
                int last = 0;
                for (LinkedHashMap<String, Object> d : items) {
                    Timestamp now = new Timestamp(System.currentTimeMillis());
                    d.put("@EntryDate", now);
                    d.put("@ModifyDate", now);
                    last = toInt(walk(con, "[dbo].[USP_ProductionOutputAllocationWithExportInvoice_InsertAndUpdate]", d));
                }
                con.commit();
                return last;
            } catch (SQLException | RuntimeException e) {
                try { con.rollback(); } catch (SQLException ignored) { /* the original error matters */ }
                throw e;
            } finally {
                try { con.setAutoCommit(auto); } catch (SQLException ignored) { }
            }
        });
    }

    /* ============================================================================== plumbing */

    private List<Map<String, Object>> rows(String proc, LinkedHashMap<String, Object> params) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            List<Map<String, Object>> out = new ArrayList<>();
            run(con, proc, params, out);
            return out;
        });
    }

    private static Object walk(Connection con, String proc, LinkedHashMap<String, Object> params)
            throws SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        run(con, proc, params, out);
        if (out.isEmpty()) return null;
        Map<String, Object> first = out.get(0);
        return first.isEmpty() ? null : first.values().iterator().next();
    }

    private static void run(Connection con, String proc, LinkedHashMap<String, Object> params,
                            List<Map<String, Object>> firstResult) throws SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;            /* .NET null: not supplied */
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        try (PreparedStatement ps = con.prepareStatement(sql.toString())) {
            for (int i = 0; i < values.size(); i++) bind(ps, i + 1, values.get(i));
            boolean isResult = ps.execute();
            boolean captured = false;
            while (true) {
                if (isResult) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            if (captured) continue;
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) row.put(md.getColumnLabel(c), plain(rs.getObject(c)));
                            firstResult.add(row);
                        }
                    }
                    captured = true;
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isResult = ps.getMoreResults();
            }
        }
    }

    private static void bind(PreparedStatement ps, int i, Object v) throws SQLException {
        if (v instanceof Integer) ps.setInt(i, (Integer) v);
        else if (v instanceof Double) ps.setDouble(i, (Double) v);
        else if (v instanceof Boolean) ps.setBoolean(i, (Boolean) v);
        else if (v instanceof Timestamp) ps.setTimestamp(i, (Timestamp) v);
        else if (v instanceof String) ps.setNString(i, (String) v);
        else if (v instanceof Long) ps.setLong(i, (Long) v);
        else if (v instanceof BigDecimal) ps.setBigDecimal(i, (BigDecimal) v);
        else if (v instanceof Number) ps.setDouble(i, ((Number) v).doubleValue());
        else if (v == null) ps.setNull(i, Types.NULL);
        else ps.setObject(i, v);
    }

    private static Object plain(Object o) {
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime().toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof BigDecimal) return ((BigDecimal) o).doubleValue();
        return o;
    }

    public static LinkedHashMap<String, Object> p(Object... kv) {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put((String) kv[i], kv[i + 1]);
        return m;
    }

    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        Object v = row.get(name);
        if (v != null || row.containsKey(name)) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Integer) return (Integer) o;
        if (o instanceof Number) return (int) Math.rint(((Number) o).doubleValue());
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }
}
