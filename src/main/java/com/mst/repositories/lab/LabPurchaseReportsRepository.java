package com.mst.repositories.lab;

import java.math.BigDecimal;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

/**
 * Procedure calls of the three Lab Report (module 1011) purchase screens:
 *
 *   626 LabDataVehicleWiseByParent   "Purchase Analysis By Vehicle"
 *   633 InvLabPurchaseRegister       "Purchase Analylsis Report"
 *   630 LabPurchaseAnalyticPeriodic  "Lab Purchase Analysis Periodic Report" (+ its two drill-down forms)
 *
 * Every EXEC carries exactly the parameters the desktop BLL adds (BLL = projects/architecture.bll/
 * 0403_Architecture.BLL.Lab.InvLabAnalysisPurchaseHeader.cs, ":NNN" below are its lines). The BLL adds most
 * parameters only when they are non-zero / non-empty; {@link Call#opt} reproduces that, so a procedure default
 * (NULL = "no filter") is used exactly where the desktop leaves the parameter out.
 *
 * Result sets are read with their column ORDER kept (LinkedHashMap): screen 633 addresses the procedure's
 * columns by ordinal (InvLabPurchaseRegister.cs:329-336, :366), and 626 reads two result sets (DataSet).
 */
@Repository
public class LabPurchaseReportsRepository {

    private final JdbcTemplate jdbc;

    public LabPurchaseReportsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** "EXEC proc @A=?, @B=?" built from the parameters actually sent. */
    public static final class Call {
        private final StringBuilder sql;
        private final List<Object> args = new ArrayList<>();
        private boolean first = true;

        public Call(String proc) { sql = new StringBuilder("EXEC ").append(proc); }

        /** A parameter the BLL always adds. */
        public Call add(String name, Object value) {
            sql.append(first ? " " : ", ").append('@').append(name).append("=?");
            first = false;
            args.add(value);
            return this;
        }

        /** A parameter the BLL adds only under its "if (...)" guard. */
        public Call opt(boolean send, String name, Object value) { return send ? add(name, value) : this; }
    }

    // ------------------------------------------------------------------------------------------ shared
    /**
     * InvLabAnalysisPurchaseHeader.GetDataForDropDownFromLabPurchaseAnaylsis (:573-644) ->
     * [dbo].[USP_GetDataForDropDownFromLabPurchaseAnaylsis]: @OrganizationId, @CompanyId always; @Activity,
     * @DocumentTypeIds, @BranchesIds only when non-empty; @ParentId only when non-zero. (@PageSize / @PageNumber /
     * @Keyword are never set by these forms.) Rows: Id, ReferenceName, Activity.
     */
    public List<Map<String, Object>> dropDownData(int organizationId, int companyId, String activity, String documentTypeIds,
                                                  String branchesIds, int parentId) {
        Call c = new Call("[dbo].[USP_GetDataForDropDownFromLabPurchaseAnaylsis]")
                .add("OrganizationId", organizationId)
                .add("CompanyId", companyId)
                .opt(activity != null && !activity.isEmpty(), "Activity", activity)
                .opt(documentTypeIds != null && !documentTypeIds.isEmpty(), "DocumentTypeIds", documentTypeIds)
                .opt(branchesIds != null && !branchesIds.isEmpty(), "BranchesIds", branchesIds)
                .opt(parentId != 0, "ParentId", parentId);
        return first(c);
    }

    // ------------------------------------------------------------------------------------------ 626
    /**
     * InvLabAnalysisPurchaseHeader.LabDataVehicleWiseByParent (:1674-1798) -> usp_getLabDataVehicleWiseByParentId
     * (GetDataSetProc: two result sets - detail, summary). Parameter order and guards as in the BLL.
     */
    public List<List<Map<String, Object>>> labDataVehicleWiseByParent(int organizationId, int companyId, int parentCategoryId,
            Timestamp fromDate, Timestamp toDate, int itemId, int supplierCustomerId, int moistureTo, int moistureFrom,
            String orderNo, boolean sendOrderNo, int slabFrom1, int slabFrom2, int slabFrom3, int slabTo1, int slabTo2, int slabTo3) {
        Call c = new Call("dbo.usp_getLabDataVehicleWiseByParentId")
                .add("OrganizationId", organizationId)                       // :1679
                .add("CompanyId", companyId)                                 // :1684
                .add("ParentCategoryId", parentCategoryId)                   // :1689 (always)
                .opt(fromDate != null, "FromDate", fromDate)                 // :1694 !CheckDateTimeNull
                .opt(toDate != null, "ToDate", toDate)                       // :1702
                .opt(itemId != 0, "ItemId", itemId)                          // :1710
                .opt(supplierCustomerId != 0, "SupplierCustomerId", supplierCustomerId)   // :1718
                .opt(moistureTo != 0, "MoisuteTo", moistureTo)               // :1726 (sic) obj.ToDocNo
                .opt(moistureFrom != 0, "MoisuteFrom", moistureFrom)         // :1734 (sic) obj.FromDocNo
                .opt(sendOrderNo, "OrderNo", orderNo)                        // :1742 Conversion.ToInt(obj.OrderNo) != 0
                .opt(slabFrom1 != 0, "MoistureSlab1From", slabFrom1)         // :1750
                .opt(slabFrom2 != 0, "MoistureSlab2From", slabFrom2)         // :1758
                .opt(slabFrom3 != 0, "MoistureSlab3From", slabFrom3)         // :1766
                .opt(slabTo1 != 0, "MoistureSlab1To", slabTo1)               // :1774
                .opt(slabTo2 != 0, "MoistureSlab2To", slabTo2)               // :1782
                .opt(slabTo3 != 0, "MoistureSlab3To", slabTo3);              // :1790
        return all(c);
    }

    // ------------------------------------------------------------------------------------------ 633
    /**
     * InvLabAnalysisPurchaseHeader.LabPurchaseAnalysisRegister (:1452-1539) -> USP_LabPurchaseAnalysis_Register.
     * isAccepted == null is the BLL's ApprovedFilter == "All" (parameter left out).
     */
    public List<Map<String, Object>> labPurchaseAnalysisRegister(int organizationId, int companyId, Timestamp fromDate,
            Timestamp toDate, int gpSrNoFrom, int gpSrNoTo, int itemId, int supplierCustomerId, int purchaseOrderId,
            Boolean isAccepted, int parentCategoryId) {
        Call c = new Call("dbo.USP_LabPurchaseAnalysis_Register")
                .add("OrganizationId", organizationId)                       // :1459
                .add("CompanyId", companyId)                                 // :1464
                .opt(fromDate != null, "Datefrom", fromDate)                 // :1467
                .opt(toDate != null, "DateTo", toDate)                       // :1475
                .opt(gpSrNoFrom != 0, "GpSrNoFrom", gpSrNoFrom)              // :1483
                .opt(gpSrNoTo != 0, "GpSrNoTo", gpSrNoTo)                    // :1491
                .opt(itemId != 0, "ItemId", itemId)                          // :1499
                .opt(supplierCustomerId != 0, "SupplierCustomerId", supplierCustomerId)   // :1507
                .opt(purchaseOrderId != 0, "PurchaseOrderId", purchaseOrderId)            // :1515
                .opt(isAccepted != null, "IsAccepted", isAccepted)           // :1523 ApprovedFilter != "All"
                .opt(parentCategoryId != 0, "InventoryParentCategorId", parentCategoryId);  // :1531 (sic)
        return first(c);
    }

    // ------------------------------------------------------------------------------------------ 630
    /** BLL.SystemUtilities.SeasonYearSchedule.GetAll (0101_...SeasonYearSchedule.cs:26-39) -> USP_SeasonYearSchedule_GetAll. */
    public List<Map<String, Object>> seasonYearSchedule(int organizationId, int companyId) {
        return first(new Call("dbo.USP_SeasonYearSchedule_GetAll").add("OrganizationId", organizationId).add("CompanyId", companyId));
    }

    /**
     * InvLabAnalysisPurchaseHeader.LabPurchaseAnalyticsA (:127-180) -> usp_LabPurchaseAnalyticsA. The form never sets
     * ItemId / BranchesId, so @ItemId / @BranchesId are left out exactly as the BLL leaves them out.
     */
    public List<Map<String, Object>> labPurchaseAnalyticsA(int organizationId, int companyId, java.sql.Date fromDate,
            java.sql.Date toDate, java.sql.Date seasonStart, java.sql.Date seasonEnd) {
        return first(new Call("dbo.usp_LabPurchaseAnalyticsA")
                .add("OrganizationId", organizationId).add("CompanyId", companyId)
                // @ItemId / @BranchesId: BLL guards "!= 0" (:162, :170) and the form never sets them - not sent (procedure default NULL)
                .add("FromDate", fromDate).add("Todate", toDate)
                .add("SeasonStart", seasonStart).add("SeasonEnd", seasonEnd));
    }

    /** InvLabAnalysisPurchaseHeader.LabPurchaseAnalyticsItemWise (:71-125) -> [dbo].[usp_LabPurchaseAnalytics_ItemWise]. */
    public List<Map<String, Object>> labPurchaseAnalyticsItemWise(int organizationId, int companyId, int inventoryParentId,
            java.sql.Date fromDate, java.sql.Date toDate, java.sql.Date seasonStart, java.sql.Date seasonEnd) {
        return first(new Call("[dbo].[usp_LabPurchaseAnalytics_ItemWise]")
                .add("OrganizationId", organizationId).add("CompanyId", companyId)
                // @ItemId: BLL guard "!= 0" (:111), never set by the form - not sent (procedure default NULL)
                .add("FromDate", fromDate).add("Todate", toDate)
                .add("SeasonStart", seasonStart).add("SeasonEnd", seasonEnd)
                .add("InventoryParentId", inventoryParentId));
    }

    /** InvLabAnalysisPurchaseHeader.LabPurchaseAnalyticsPartyWiseByItemId (:15-69) -> [dbo].[usp_LabPurchaseAnalyticsPartyWiseByItemId]. */
    public List<Map<String, Object>> labPurchaseAnalyticsPartyWiseByItemId(int organizationId, int companyId,
            java.sql.Date fromDate, java.sql.Date toDate, java.sql.Date seasonStart, java.sql.Date seasonEnd, int itemId, int sortNo) {
        return first(new Call("[dbo].[usp_LabPurchaseAnalyticsPartyWiseByItemId]")
                .add("OrganizationId", organizationId).add("CompanyId", companyId)
                .add("ItemId", itemId)
                .add("FromDate", fromDate).add("Todate", toDate)
                .add("SeasonStart", seasonStart).add("SeasonEnd", seasonEnd)
                .opt(sortNo != 0, "SortNo", sortNo));
    }

    // ------------------------------------------------------------------------------------------ execution
    private List<Map<String, Object>> first(Call c) {
        List<List<Map<String, Object>>> sets = all(c);
        return sets.isEmpty() ? new ArrayList<>() : sets.get(0);
    }

    /** DataAdapter.Fill: every result set, rows as ordered maps; update counts are skipped. */
    private List<List<Map<String, Object>>> all(Call c) {
        final Object[] args = c.args.toArray();
        return jdbc.execute(c.sql.toString(), (PreparedStatementCallback<List<List<Map<String, Object>>>>) ps -> {
            for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
            List<List<Map<String, Object>>> sets = new ArrayList<>();
            boolean isResultSet = ps.execute();
            while (true) {
                if (isResultSet) {
                    try (ResultSet rs = ps.getResultSet()) { sets.add(read(rs)); }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isResultSet = ps.getMoreResults();
            }
            return sets;
        });
    }

    private static List<Map<String, Object>> read(ResultSet rs) throws SQLException {
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        String[] names = new String[n];
        for (int i = 0; i < n; i++) names[i] = md.getColumnLabel(i + 1);
        List<Map<String, Object>> rows = new ArrayList<>();
        while (rs.next()) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (int i = 0; i < n; i++) row.put(names[i], plain(rs.getObject(i + 1)));
            rows.add(row);
        }
        return rows;
    }

    /**
     * JSON-safe cell: dates as ISO text; binary cells (the register procedure's CompLogoImage) are not carried to
     * the page - the print layer adds the company logo itself.
     */
    private static Object plain(Object v) {
        if (v == null || v instanceof String || v instanceof Integer || v instanceof Long || v instanceof Double
                || v instanceof Float || v instanceof Short || v instanceof Byte || v instanceof Boolean || v instanceof BigDecimal) return v;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString().length() == 16
                ? ((Timestamp) v).toLocalDateTime().withNano(0).toString() + ":00" : ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof byte[]) return null;
        if (v instanceof Number) return v;
        return String.valueOf(v);
    }
}
