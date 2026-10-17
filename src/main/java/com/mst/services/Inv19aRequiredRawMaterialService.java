package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.ColumnMapRowMapper;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.RowMapperResultSetExtractor;
import org.springframework.stereotype.Service;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Timestamp;
import java.util.*;

/**
 * RequiredRawMaterialAgainstSaleOrders (Architecture.WinApp.Inventory_Reports/RequiredRawMaterialAgainstSaleOrders.cs) -
 * ScreenDefinition 294 "Required Raw Material Against Sale Orders" (Inventory hub, module 19 Stock Reports). Line refs ":NNN" are that file.
 * Data: BLL SaleOrderReports.MRPAgainstSalesOrdersRice -> dbo.USP_MRPAgainstSalesOrdersRice (@OrganizationId,@CompanyId,@ToDate) - TWO result sets
 * (Tables[0] = #TmpMRPFinal brand-wise rows, Tables[1] = raw item totals sItemId/ItemNameRm/RequiredWeight);
 * SaleOrderReports.GetAvailableStockFromTransactions -> dbo.USP_GetAvailableStockFromTransactions (@OrganizationId,@CompanyId,@DocDate,@ItemIds) (ItemId, BalQty, BalWeight).
 * (No BLL/DAL source was recovered for these two; the procedure names are the only ones in procdure_index.csv matching the DAL method names
 * and the Crystal registry lists 463/464 against this form.)
 */
@Service
public class Inv19aRequiredRawMaterialService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public Inv19aRequiredRawMaterialService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static double dbl(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null || String.valueOf(o).isBlank()) return 0d;
        try { return Double.parseDouble(String.valueOf(o).trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }
    private static int num(Object o) { return (int) dbl(o); }
    private static Timestamp day(String iso) { return Timestamp.valueOf(iso.substring(0, 10) + " 00:00:00"); }

    /** Runs a procedure that returns several result sets (DataSet ds = ...; ds.Tables[0], ds.Tables[1]). */
    private List<List<Map<String, Object>>> multi(final String sql, final Object... args) {
        return jdbc.execute((ConnectionCallback<List<List<Map<String, Object>>>>) con -> {
            try (PreparedStatement ps = con.prepareStatement(sql)) {
                for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
                List<List<Map<String, Object>>> out = new ArrayList<>();
                RowMapperResultSetExtractor<Map<String, Object>> ex = new RowMapperResultSetExtractor<>(new ColumnMapRowMapper());
                boolean hasRs = ps.execute();
                while (true) {
                    if (hasRs) { try (ResultSet rs = ps.getResultSet()) { out.add(ex.extractData(rs)); } }
                    else if (ps.getUpdateCount() == -1) break;
                    hasRs = ps.getMoreResults();
                }
                return out;
            }
        });
    }

    /** RequiredRawMaterialAgainstSaleOrders_Load :88 - no lookups; datToDate default = today, txtqtyUom = "100" (designer :710). */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("toDate", java.time.LocalDate.now().toString());
        out.put("packUom", 100);
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        return out;
    }

    /**
     * GridFill :121 + UpdateBal :394.
     * history  = ds.Tables[0] rows reshaped :141-155 (Id=BomHId, ItemId=BomHItemId, OrderItem=BomHItemName, OrderUom=BomHItemUomName, OrderQty, OrderWeight,
     *            RawItem=ItemNameRm, RawPackUom=UomRm, RequiredWt=RequiredWeight, Equivalent, RawPrct=PercRm);
     * historyRaw = ds.Tables[0] untouched (dtHistory - what 464-RequiredRawMaterialRegisterTwo.rpt prints);
     * rawItems = ds.Tables[1] -> ItemId=sItemId, ItemName=ItemNameRm, RequiredGrossWt=RequiredWeight, RequiredGrossQty=RequiredWeight/PackUom :171,
     *            then UpdateBal: AvailableStockWeight = BalWeight of the matching USP_GetAvailableStockFromTransactions row, NetRequiredWeight = RequiredGrossWt - BalWeight
     *            (rows without a match keep 0 / 0 :421-428).
     */
    public Map<String, Object> show(Map<String, Object> p) {
        String toDate = str(p.get("toDate"));
        if (toDate.isEmpty()) throw new IllegalArgumentException("To Date is required.");
        double packUom = dbl(p.get("packUom"));
        if (packUom <= 0) packUom = 100;   // Conversion.ToDouble("") = 0 would divide by zero; the Leave handler resets to 100 :443
        List<List<Map<String, Object>>> ds = multi("EXEC dbo.USP_MRPAgainstSalesOrdersRice @OrganizationId=?,@CompanyId=?,@ToDate=?",
                context.currentOrganizationId(), context.currentCompanyId(), day(toDate));
        List<Map<String, Object>> t0 = ds.size() > 0 ? ds.get(0) : new ArrayList<>();
        List<Map<String, Object>> t1 = ds.size() > 1 ? ds.get(1) : new ArrayList<>();

        List<Map<String, Object>> history = new ArrayList<>();
        for (Map<String, Object> r : t0) {
            Map<String, Object> h = new LinkedHashMap<>();
            h.put("Id", r.get("BomHId")); h.put("ItemId", r.get("BomHItemId")); h.put("OrderItem", r.get("BomHItemName")); h.put("OrderUom", r.get("BomHItemUomName"));
            h.put("OrderQty", dbl(r.get("OrderQty"))); h.put("OrderWeight", dbl(r.get("OrderWeight"))); h.put("RawItem", r.get("ItemNameRm")); h.put("RawPackUom", r.get("UomRm"));
            h.put("RequiredWt", dbl(r.get("RequiredWeight"))); h.put("Equivalent", dbl(r.get("Equivalent"))); h.put("RawPrct", dbl(r.get("PercRm")));
            history.add(h);
        }

        List<Map<String, Object>> raw = new ArrayList<>();
        List<String> itemIds = new ArrayList<>();
        for (Map<String, Object> r : t1) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("ItemId", r.get("sItemId")); x.put("ItemName", r.get("ItemNameRm"));
            x.put("RequiredGrossWt", dbl(r.get("RequiredWeight"))); x.put("RequiredGrossQty", dbl(r.get("RequiredWeight")) / packUom);
            x.put("AvailableStockWeight", 0d); x.put("NetRequiredWeight", 0d);
            raw.add(x); itemIds.add(str(r.get("sItemId")));
        }
        if (!raw.isEmpty()) {   // UpdateBal :394
            var bal = jdbc.queryForList("EXEC dbo.USP_GetAvailableStockFromTransactions @OrganizationId=?,@CompanyId=?,@DocDate=?,@ItemIds=?",
                    context.currentOrganizationId(), context.currentCompanyId(), day(toDate), String.join(",", itemIds));
            Map<Integer, Map<String, Object>> byItem = new HashMap<>();
            for (Map<String, Object> b : bal) byItem.putIfAbsent(num(b.get("ItemId")), b);
            for (Map<String, Object> x : raw) {
                Map<String, Object> m = byItem.get(num(x.get("ItemId")));
                if (m != null) {
                    double balWt = dbl(m.get("BalWeight"));
                    x.put("AvailableStockWeight", balWt);
                    x.put("NetRequiredWeight", dbl(x.get("RequiredGrossWt")) - balWt);
                }
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("history", history); out.put("historyRaw", t0); out.put("rawItems", raw);
        return out;
    }
}
